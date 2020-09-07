using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IIEFPSubjectRepository _iefpSubjectRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConverterHelper _converterHelper;
        private readonly IFileHelper _fileHelper;

        public object ExcelReaderFactory { get; private set; }

        public SubjectsController(ISubjectRepository subjectRepository, IIEFPSubjectRepository iefpSubjectRepository,
            ICourseRepository courseRepository, ITeacherRepository teacherRepository, IHostingEnvironment hostingEnvironment,
            IConverterHelper converterHelper, IFileHelper fileHelper)
        {
            _subjectRepository = subjectRepository;
            _iefpSubjectRepository = iefpSubjectRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _hostingEnvironment = hostingEnvironment;
            _converterHelper = converterHelper;
            _fileHelper = fileHelper;
        }

        // GET: SubjectsController
        public IActionResult Index()
        {
            return View(_subjectRepository.GetAll());
        }

        // GET: SubjectsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            subject.Course = await _courseRepository.GetByIdAsync(subject.CourseId);
            subject.Teacher = await _teacherRepository.GetByIdAsync(subject.TeacherId);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        public IActionResult CreateIndex()
        {
            var model = new SubjectViewModel
            {
                Courses = _courseRepository.GetAll().Where(c => c.Id > 1),
                Teachers = _teacherRepository.GetAll().Where(c => c.Id > 1),
                IEFPSubjects = _iefpSubjectRepository.GetAll().Where(e => e.Field == "Áudiovisuais e Produção dos Media" ||
                e.Field == "Ciências Informáticas" || e.Field == "Eletrónica e Automação").ToList()//Filter by Field                
            };

            IEnumerable<Subject> Subjects = _subjectRepository.GetAll();

            List<IEFPSubject> ISubjects = new List<IEFPSubject>(model.IEFPSubjects);

            foreach(var iefpsubject in ISubjects)
            {
                foreach (var subject in Subjects)
                {
                    if (iefpsubject.Code == subject.Code)
                        model.IEFPSubjects.Remove(iefpsubject);
                }
            }

            //model.Courses = _courseRepository.GetAll();
            //model.Teachers = _teacherRepository.GetAll();
            //model.IEFPSubjects = _iefpSubjectRepository.GetAll().Where(e => e.Field == "Áudiovisuais e Produção dos Media" || e.Field == "Ciências Informáticas" || e.Field == "Eletrónica e Automação");//Filter by Field

            return View(model);
        }

        //GET: SubjectsController/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iefpsubject = await _iefpSubjectRepository.GetByIdAsync(id.Value);

            var subject = _converterHelper.ConvertToSubject(iefpsubject, 1, await _courseRepository.GetByIdAsync(1),
                1, await _teacherRepository.GetByIdAsync(1), "0", true);

            if (subject == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToSubjectViewModel(subject, _courseRepository.GetAll().Where(c => c.Id > 1), _teacherRepository.GetAll().Where(c => c.Id > 1));

            return View(model);
        }

        // POST: ClassesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var iefpsubject = await _iefpSubjectRepository.GetByIdAsync(model.Id);

                var subject = _converterHelper.ConvertToSubject(iefpsubject, model.CourseId, await _courseRepository.GetByIdAsync(model.CourseId),
                model.TeacherId, await _teacherRepository.GetByIdAsync(model.TeacherId), model.Credits, true);

                if (!await _subjectRepository.ExistsCodeAsync(subject.Code))
                {
                    await _subjectRepository.CreateAsync(subject);
                    return RedirectToAction(nameof(Index));
                }

                //await _subjectRepository.CreateAsync(subject);
                //return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: SubjectsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);

            if (subject == null)
            {
                return NotFound();
            }

            subject.Course = await _courseRepository.GetByIdAsync(subject.CourseId);
            subject.Teacher = await _teacherRepository.GetByIdAsync(subject.TeacherId);

            var model = _converterHelper.ToSubjectViewModel(subject, _courseRepository.GetAll().Where(c => c.Id > 1), _teacherRepository.GetAll().Where(c => c.Id > 1));

            //var model = _converterHelper.ToSubjectViewModel(subject);
            //model.Courses = _courseRepository.GetAll();
            //model.Teachers = _teacherRepository.GetAll();

            return View(model);
        }

        // POST: SubjectsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var subject = await _subjectRepository.GetByIdAsync(model.Id);
                    //subject.Course = await _courseRepository.GetByIdAsync(model.CourseId);
                    //subject.Teacher = await _teacherRepository.GetByIdAsync(model.TeacherId);

                    var subject = _converterHelper.ToSubject(model, await _courseRepository.GetByIdAsync(model.CourseId),
                        await _teacherRepository.GetByIdAsync(model.TeacherId), true);

                    subject.Id = model.Id;

                    await _subjectRepository.UpdateAsync(subject);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _subjectRepository.ExistsAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "UploadExcel";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<div class='panel-body'><table class='table table-hover table-responsive table-striped' style='width:100%; height:400px' id='MyTable'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                        {
                            continue;
                        }

                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }

                        if (row.Cells.All(d => d.CellType == CellType.Blank))
                        {
                            continue;
                        }

                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                            }
                        }

                        var iefpsubject = new IEFPSubject
                        {
                            Code = row.GetCell(0).ToString(),
                            Name = row.GetCell(1).ToString(),
                            Duration = row.GetCell(2).ToString(),
                            ReferenceCode = row.GetCell(3).ToString(),
                            FieldCode = row.GetCell(4).ToString(),
                            Field = row.GetCell(5).ToString(),
                            Reference = row.GetCell(6).ToString(),
                            QNQLevel = row.GetCell(7).ToString(),
                            QEQLevel = row.GetCell(8).ToString(),
                            Component = row.GetCell(9).ToString()
                        };

                        if (!await _iefpSubjectRepository.ExistsCodeAsync(iefpsubject.Code))
                            await _iefpSubjectRepository.CreateAsync(iefpsubject);

                        sb.AppendLine("</tr>");
                    }
                    sb.Append("</table></div>");
                }
            }
            return this.Content(sb.ToString());
        }

        public IActionResult Download()
        {
            string Files = "wwwroot/UploadExcel/CoreProgramm_ExcelImport.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "employee.xlsx");
        }
    }
}
