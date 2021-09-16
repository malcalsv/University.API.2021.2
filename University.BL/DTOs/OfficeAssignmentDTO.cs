namespace University.BL.DTOs
{
    public class OfficeAssignmentDTO
    {       
        public int InstructorID { get; set; }
        
        public string Location { get; set; }

        public InstructorDTO Instructor { get; set; }
    }
}
