using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]

        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var stu = await connection.QueryAsync<Student>("select * from S_Table");
            return Ok(stu);
        }

        [HttpGet("{StuId}")]
        public async Task<ActionResult<List<Student>>> GetStudents(int StuId)
        {

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var student = await connection.QueryFirstAsync<Student>("select * from StudentsTb where studentid=@StudentID",
                new { StudentID = StuId });
            return Ok(student);
        }
        [HttpPost]
        public async Task<ActionResult<List<Student>>> CreateStudents(Student students)
        {

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into StudentsTb (firstname,lastname,address,mobile)values(@FirstName,@LastName,@Address,@Mobile)", students);
            return Ok(await SelectAllStudents(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Student>>> UpdateStudents(Student students)
        {

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update  StudentsTb set firstname=@FirstName,lastname=@LastName,address=@Address,mobile=@Mobile where studentid=@StudentID", students);
            return Ok(await SelectAllStudents(connection));
        }

        [HttpDelete("{StuId}")]
        public async Task<ActionResult<List<Student>>> DeleteStudents(int StuId)
        {

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from  StudentsTb where studentid=@StudentID", new { StudentID = StuId });
            return Ok(await SelectAllStudents(connection));
        }

        private static async Task<IEnumerable<Student>> SelectAllStudents(SqlConnection connection)
        {
            return await connection.QueryAsync<Student>("select * from StudentsTb ");
        }

    }
}

