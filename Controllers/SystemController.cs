using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OLEToolBoxWebAPIPruebas.classes;
using OLEToolBoxWebAPIPruebas.DTOs.DTOMainCompany;
using OLEToolBoxWebAPIPruebas.DTOs.DTOSystemData;
using OLEToolBoxWebAPIPruebas.Models;
using System.Runtime.Intrinsics;

namespace OLEToolBoxWebAPIPruebas.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
        {


        private PruebasoletoolboxContext _context;


        public SystemController(PruebasoletoolboxContext context)
            {
            _context = context;
            }

        /**
         * 
         * Método GetMainCompanyData()
         * 
         * Devuelve los datos de la empresa en la que se ha incorporado el software.
         * 
         * */


        [HttpGet("getcompanies")]
        public async Task<ActionResult> GetCompaniesData()
            {

            var mainCompany = await _context.MainCompanies.ToListAsync();

            return Ok(mainCompany);

            }

        [HttpGet("getmaincompany")]
        public async Task<ActionResult> GetMainCompanyData()
            {

            var mainCompany = await _context.MainCompanies.FirstOrDefaultAsync();

            if (mainCompany is null)
                {

                return NotFound("No hay datos de la compañía principal.");

                }

            return Ok(mainCompany);

            }

        /**
         * 
         * Método PostMainCompanyData([FromBody]DTOMainCompanyPost company)
         * 
         * Inserta los datos de la empresa en la que se ha incorporado el software.
         * 
         * */

        [HttpPost("postcompany")]
        public async Task<ActionResult> PostMainCompanyData([FromBody] DTOMainCompanyPost company)
            {

            var companySaved = await _context.MainCompanies.ToListAsync();

            if (companySaved.Count() != 0)
                {

                return BadRequest("Ya hay datos de la compañía principal.");

                }

            MainCompany mainCompany = new MainCompany()
                {
                MainId = Generators.GenerateIDMainCompany(),
                MainCompanyName = company.MainCompanyNameDTO,
                DateFund = company.DateFundDTO,
                Sector = company.SectorDTO,
                TotalWorkers = company.TotalWorkersDTO,
                Size = company.SizeDTO,

                };

            await _context.AddAsync(mainCompany);
            await _context.SaveChangesAsync();

            return Created("MainCompany", new { company = mainCompany });


            }

        /**
         * 
         * Método PutMainCompany([FromBody] DTOMainCompanyPut updates)
         * 
         * Actualiza los datos de la empresa en la que se ha incorporado el software.
         * 
         * */

        [HttpPut("putcompany")]
        public async Task<ActionResult> PutMainCompany([FromBody] DTOMainCompanyPut updates)
            {

            var companyUpdate = await _context.MainCompanies.AsTracking().FirstOrDefaultAsync();
            if (companyUpdate == null)
                {
                return NotFound("No hay datos de la compañía.");
                }
            companyUpdate.MainCompanyName = updates.MainComanyNameDTO;
            companyUpdate.DateFund = updates.DateFundDTO;
            companyUpdate.Sector = updates.SectorDTO;
            companyUpdate.TotalWorkers = updates.TotalWorkersDTO;
            companyUpdate.Size = updates.SizeDTO;

            _context.Update(companyUpdate);

            await _context.SaveChangesAsync();
            return Accepted("Datos actualizados");
            }

        [HttpDelete("deletemaincompanydata")]
        public async Task<ActionResult> DeleteMainCompanyData()
            {

            //var data = await _context.MainCompanies.Where(c => c.MainId.Equals(id)).FirstOrDefaultAsync();
            var data = await _context.MainCompanies.FirstOrDefaultAsync();

            if (data is null)
                {
                return NotFound("No hay datos que eliminar");
                }

            _context.Remove(data);
            await _context.SaveChangesAsync();
            return Ok(data);
            }

        /**
         * 
         * Método GetSystemData()
         * 
         * Devuelve los datos del sistema OLE Toolbox.
         * 
         * */

        //"Dato" del sistema? ajaja

        [HttpGet("getsystemdata")]
        public async Task<ActionResult<SystemDatum>> GetSystemData()
            {

            var systemData = await _context.SystemData.FirstOrDefaultAsync();

            if (systemData == null) return NotFound("No hay datos de sistema.");

            return Ok(systemData);

            }

        /**
         * 
         * Método PostSystemData([FromBody] DTOSystemDataPost data)
         * 
         * Inserta los datos del sistema OLE Toolbox.
         * 
         * */

        [HttpPost("postsystemdata")]
        public async Task<ActionResult> PostSystemData([FromBody] DTOSystemDataPost data)
            {

            var dataStoraged = await _context.SystemData.ToListAsync();

            if (dataStoraged.Count() != 0)
                {

                return BadRequest("Ya hay datos de sistema almacenados en la base de datos.");

                }

            SystemDatum systemData = new SystemDatum()
                {
                SystemName = data.SystemNameDTO,
                DateStart = DateTime.UtcNow,
                SystemVersion = "v1.0",
                DateNow = DateTime.UtcNow,
                };

            await _context.AddAsync(systemData);
            await _context.SaveChangesAsync();

            return Created("SystemData ", new { system = systemData });


            }
        /**
         * 
         * Método PutSystemData([FromBody] DTOMainCompanyPut updates)
         * 
         * Actualiza los datos de sistema OLE ToolBox.
         * 
         * */

        [HttpPut("putsystemdata")]
        public async Task<ActionResult> PutSystemData([FromBody] DTOSystemDataPut updates)
            {

            var systemUpdate = await _context.SystemData.AsTracking().FirstOrDefaultAsync();
            if (systemUpdate == null)
                {
                return NotFound("No hay datos de sistema en la base de datos.");
                }
            systemUpdate.SystemName = updates.SystemNameDTO;
            systemUpdate.DateStart = updates.DateStartDTO;
            systemUpdate.SystemVersion = updates.SystemVersionDTO;
            systemUpdate.TotalUsers = updates.TotalUsersDTO;
            systemUpdate.Licensed = updates.LicensedDTO;
            systemUpdate.DateNow = DateTime.UtcNow;


            _context.Update(systemUpdate);

            await _context.SaveChangesAsync();
            return Accepted("Datos actualizados");
            }

        [HttpDelete("deletesystemdata/{id:int}")]
        public async Task<ActionResult> DeleteSystemData(int id)
            {

            var data = await _context.SystemData.FindAsync(id);

            if (data is null)
                {
                return NotFound("No hay datos que eliminar");
                }

            _context.Remove(data);
            await _context.SaveChangesAsync();
            return Ok("Datos de sistema eliminados");
            }

        }
    }
