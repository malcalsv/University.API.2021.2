using System;
using System.Linq;
using System.Web.Http;
using University.BL.DTOs;
using University.BL.Models;

namespace University.API.Controllers
{
    [RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        private readonly UniversityEntities context = new UniversityEntities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var students = context.Student.ToList();
            var studentsDTO = students.Select(x => new StudentDTO
            {
                ID = x.ID,
                FirstMidName = x.FirstMidName,
                LastName = x.LastName,
                EnrollmentDate = x.EnrollmentDate.Value
            }).ToList();

            return Ok(studentsDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            var student = context.Student.Find(id);
            var studentDTO = new StudentDTO
            {
                ID = student.ID,
                FirstMidName = student.FirstMidName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate.Value
            };

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
        public IHttpActionResult Create(StudentDTO studentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var student = context.Student.Add(new Student
                {
                    FirstMidName = studentDTO.FirstMidName,
                    LastName = studentDTO.LastName,
                    EnrollmentDate = studentDTO.EnrollmentDate
                });
                context.SaveChanges();

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
        public IHttpActionResult Edit(int id, StudentDTO studentDTO)
        {
            try
            {
                if (id != studentDTO.ID)
                    return BadRequest();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var student = context.Student.Find(id);
                if (student == null) return NotFound();

                student.LastName = studentDTO.LastName;
                student.FirstMidName = studentDTO.FirstMidName;
                student.EnrollmentDate = studentDTO.EnrollmentDate;

                context.SaveChanges();

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
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var student = context.Student.Find(id);
                if (student == null) return NotFound();

                if (context.Enrollment.Any(x => x.StudentID == id))
                    throw new Exception("Dependencies");

                context.Student.Remove(student);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
