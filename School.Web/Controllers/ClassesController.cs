using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using Syncfusion.EJ2.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IClassRepository _classRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IConverterHelper _converterHelper;

        public ClassesController(IClassRepository classRepository, ICourseRepository courseRepository, ITeacherRepository teacherRepository,
            IStudentRepository studentRepository, ISubjectRepository subjectRepository, IScheduleRepository scheduleRepository,
            IClassroomRepository classroomRepository, IConverterHelper converterHelper)
        {
            _classRepository = classRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
            _scheduleRepository = scheduleRepository;
            _classroomRepository = classroomRepository;
            _converterHelper = converterHelper;
        }

        // GET: ClassesController
        public IActionResult Index()
        {
            return View(_classRepository.GetAll().Where(a => a.isActive == true)
                .Include(c => c.Course).Where(a => a.isActive == true)
                .Include(c => c.Classroom).Where(a => a.isActive == true)
                .Include(c => c.Schedule).Where(a => a.isActive == true));
        }

        [Authorize(Roles = "Admin, Teacher")]
        // GET: ClassesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToClassViewModel(classes, await _courseRepository.GetByIdAsync(classes.CourseId),
                _studentRepository.GetAll().Where(e => e.ClassId == id.Value).Where(a => a.isActive == true),
                _subjectRepository.GetAll().Where(e => e.CourseId == classes.CourseId).Where(a => a.isActive == true));

            model.Teachers = _classRepository.GetTeachers(model, _teacherRepository.GetAll().Where(a => a.isActive == true), model.Subjects).ToList();

            var students = _studentRepository.GetAll().Where(e => e.ClassId == id.Value).Where(a => a.isActive == true);
            model.Students = students;

            model.Schedule = await _scheduleRepository.GetByIdAsync(classes.ScheduleId);
            model.Classroom = await _classroomRepository.GetByIdAsync(classes.ClassroomId);

            if (classes == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: ClassesController/Create
        public IActionResult Create()
        {
            var model = new ClassViewModel
            {
                Courses = _courseRepository.GetAll().Where(c => c.Id > 1).Where(a => a.isActive == true).ToList(),
                Students = _studentRepository.GetAll().Where(a => a.isActive == true).ToList(),
                Classrooms = _classroomRepository.GetAll().Where(a => a.isActive == true).ToList(),
                Schedules = _scheduleRepository.GetAll().Where(a => a.isActive == true).ToList()
            };

            return View(model);
        }

        // POST: ClassesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var classes = _converterHelper.ToClass(model, await _courseRepository.GetByIdAsync(model.CourseId), true);

                    await _classRepository.CreateAsync(classes);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }                
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: ClassesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);
            if (classes == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var model = _converterHelper.ToClassViewModel(classes, await _courseRepository.GetByIdAsync(classes.CourseId), _studentRepository.GetAll().Where(e => e.ClassId == id.Value).Where(a => a.isActive == true), _subjectRepository.GetAll().Where(e => e.CourseId == classes.CourseId).Where(a => a.isActive == true));

            model.Schedules = _scheduleRepository.GetAll().Where(a => a.isActive == true);
            model.Classrooms = _classroomRepository.GetAll().Where(a => a.isActive == true);

            model.Schedule = await _scheduleRepository.GetByIdAsync(classes.ScheduleId);
            model.Classroom = await _classroomRepository.GetByIdAsync(classes.ClassroomId);

            return View(model);
        }

        // POST: ClassesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var classes = _converterHelper.ToClass(model, await _courseRepository.GetByIdAsync(model.CourseId), true);

                    classes.Id = model.Id;

                    await _classRepository.UpdateAsync(classes);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _classRepository.ExistsAsync(model.Id))
                    {
                        return new NotFoundViewResult("ClassNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: ClassesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToClassViewModel(classes, await _courseRepository.GetByIdAsync(classes.CourseId),
                _studentRepository.GetAll().Where(e => e.ClassId == id.Value).Where(a => a.isActive == true),
                _subjectRepository.GetAll().Where(e => e.CourseId == classes.CourseId).Where(a => a.isActive == true));

            model.Teachers = _classRepository.GetTeachers(model, _teacherRepository.GetAll().Where(a => a.isActive == true), model.Subjects).ToList();

            model.Students = _studentRepository.GetAll().Where(e => e.ClassId == id.Value).Where(a => a.isActive == true);
            model.Schedule = await _scheduleRepository.GetByIdAsync(classes.ScheduleId);
            model.Classroom = await _classroomRepository.GetByIdAsync(classes.ClassroomId);

            if (classes == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            return View(model);
        }

        // POST: ClassesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var classes = await _classRepository.GetByIdAsync(id);
                await _classRepository.DeleteAsync(classes);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }            
        }
    }
}
