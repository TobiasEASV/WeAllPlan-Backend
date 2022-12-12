using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DatabaseController : ControllerBase
{
    private DatabaseRepo _databaseRepo;

    public DatabaseController(DatabaseRepo databaseRepo)
    {
        _databaseRepo = databaseRepo;
    }

    [HttpGet]
    [Route("CreateDB")]
    public string CreateDB()
    {
        _databaseRepo.CreateDB();
        return "Database was deleted and rebuild.";
    }
}