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

        public SubjectsController(ISubjectRepository subjectRepository, IIEFPSubjectRepository iefpSubjectRepository, ICourseRepository courseRepository, ITeacherRepository teacherRepository, IHostingEnvironment hostingEnvironment, IConverterHelper converterHelper)
        {
            _subjectRepository = subjectRepository;
            _iefpSubjectRepository = iefpSubjectRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _hostingEnvironment = hostingEnvironment;
            _converterHelper = converterHelper;
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

            var course = await _courseRepository.GetByIdAsync(subject.CourseId);
            subject.Course = course;

            var teacher = await _teacherRepository.GetByIdAsync(subject.TeacherId);
            subject.Teacher = teacher;

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
                Courses = _courseRepository.GetAll(),
                Teachers = _teacherRepository.GetAll()
            };

            model.IEFPSubjects = _iefpSubjectRepository.GetAll().Where(e => e.Field == "Áudiovisuais e Produção dos Media" || e.Field == "Ciências Informáticas" || e.Field == "Eletrónica e Automação");//Filter by Field

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

            var subject = new Subject
            {
                Code = iefpsubject.Code,
                Name = iefpsubject.Name,
                Duration = iefpsubject.Duration,
                ReferenceCode = iefpsubject.ReferenceCode,
                FieldCode = iefpsubject.FieldCode,
                Field = iefpsubject.Field,
                Reference = iefpsubject.Reference,
                QNQLevel = iefpsubject.QNQLevel,
                QEQLevel = iefpsubject.QEQLevel,
                Component = iefpsubject.Component,
                CourseId = 3,
                Course = await _courseRepository.GetByIdAsync(3),
                TeacherId = 1,
                Teacher = await _teacherRepository.GetByIdAsync(1),
            };

            if (subject == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(subject.CourseId);
            subject.Course = course;

            var teacher = await _teacherRepository.GetByIdAsync(subject.TeacherId);
            subject.Teacher = teacher;

            if (!await _subjectRepository.ExistsCodeAsync(subject.Code))
                await _subjectRepository.CreateAsync(subject);

            var model = _converterHelper.ToSubjectViewModel(subject);

            model.Courses = _courseRepository.GetAll();

            model.Teachers = _teacherRepository.GetAll();

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

                var subject = new Subject
                {
                    Id = model.Id,
                    Code = iefpsubject.Code,
                    Name = iefpsubject.Name,
                    Duration = iefpsubject.Duration,
                    Credits = model.Credits,
                    ReferenceCode = iefpsubject.ReferenceCode,
                    FieldCode = iefpsubject.FieldCode,
                    Field = iefpsubject.Field,
                    Reference = iefpsubject.Reference,
                    QNQLevel = iefpsubject.QNQLevel,
                    QEQLevel = iefpsubject.QEQLevel,
                    Component = iefpsubject.Component,
                    CourseId = model.CourseId,
                    Course = await _courseRepository.GetByIdAsync(model.CourseId),
                    TeacherId = model.TeacherId,
                    Teacher = await _teacherRepository.GetByIdAsync(model.TeacherId)
                };

                await _subjectRepository.CreateAsync(subject);
                return RedirectToAction(nameof(Index));
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

            var course = await _courseRepository.GetByIdAsync(subject.CourseId);
            subject.Course = course;

            var teacher = await _teacherRepository.GetByIdAsync(subject.TeacherId);
            subject.Teacher = teacher;

            var model = _converterHelper.ToSubjectViewModel(subject);

            model.Courses = _courseRepository.GetAll();

            model.Teachers = _teacherRepository.GetAll();

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
                    var subject = await _subjectRepository.GetByIdAsync(model.Id);

                    var course = await _courseRepository.GetByIdAsync(model.CourseId);
                    subject.Course = course;

                    var teacher = await _teacherRepository.GetByIdAsync(model.TeacherId);
                    subject.Teacher = teacher;

                    //var subject = _converterHelper.ToSubject(model, true);

                    //subject.Id = model.Id;

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
                    sb.Append("<table class='table table-bordered'><tr>");
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
                    sb.Append("</table>");
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
