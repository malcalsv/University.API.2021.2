using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using University.BL.DTOs;
using University.BL.Models;
using University.BL.Repositories.Implements;
using AutoMapper;

namespace University.API.Controllers
{
    [RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        private readonly IMapper mapper;
        private readonly StudentRepository studentRepository = new StudentRepository(new UniversityEntities());

        public StudentsController()
        {
            this.mapper = WebApiApplication.MapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            var students = await studentRepository.GetAll();
            var studentsDTO = students.Select(x => mapper.Map<StudentDTO>(x));

            return Ok(studentsDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var student = await studentRepository.GetById(id);
            var studentDTO = mapper.Map<StudentDTO>(student);

            return Ok(studentDTO);
        }

        /// <summary>
        /// Crear un objeto de estudiante
        /// </summary>
        /// <param name="studentDTO">Objeto del estudiante</param>
        /// <returns>Objeto del estudiante</returns>
        /// <response code="200">Ok. Devuelve el objeto solicitado.</response>
        /// <response code="400">BadRequest. No se cumple con la validación del modelo.</response>
        /// <response code="500">InternalServerError. Se ha presentado un error.</response>
        [HttpPost]
        public async Task<IHttpActionResult> Create(StudentDTO studentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var student = mapper.Map<Student>(studentDTO);
                student = await studentRepository.Insert(student);
                studentDTO.ID = student.ID;

                return Ok(studentDTO);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IHttpActionResult> Edit(int id, StudentDTO studentDTO)
        {
            try
            {
                if (id != studentDTO.ID)
                    return BadRequest();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var student = await studentRepository.GetById(id);
                if (student == null) return NotFound();

                //  Update Field
                student.LastName = studentDTO.LastName;
                student.FirstMidName = studentDTO.FirstMidName;
                student.EnrollmentDate = studentDTO.EnrollmentDate;

                //  Update All
                //  student = mapper.Map<Student>(studentDTO);

                await studentRepository.Update(student);

                return Ok(studentDTO);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var student = await studentRepository.GetById(id);
                if (student == null) return NotFound();

                //if (context.Enrollment.Any(x => x.StudentID == id))
                //    throw new Exception("Dependencies");

                await studentRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
