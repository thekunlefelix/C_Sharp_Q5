using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using C_Sharp_Q5.Dtos;
using C_Sharp_Q5.Dtos.Country;
using C_Sharp_Q5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using C_Sharp_Q5.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace C_Sharp_Q5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CountryController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<ReturnResponse>> GetCountries()
        {
            var countries = XDocument.Load(_hostingEnvironment.ContentRootPath + @"\CountryDb\Countries.xml");
            var allCountries = countries.Element("Countries").Elements("Country").Select(a => new Country()
            {
                CountryId = Convert.ToInt32(a.Attribute("Id").Value),
                CountryName = a.Attribute("Name").Value
            }).ToList();

            return StatusCode(StatusCodes.Status200OK, new ReturnResponse()
            {
                StatusCode = Utils.StatusCode_Success,
                StatusMessage = Utils.StatusMessage_Success,
                ObjectValue = allCountries
            });
        }

        [HttpGet("{countryId}")]
        public async Task<ActionResult<ReturnResponse>> GetCountry([FromRoute] int countryId)
        {
            var countries = XDocument.Load(_hostingEnvironment.ContentRootPath + @"\CountryDb\Countries.xml");
            var country = countries.Element("Countries").Elements("Country").Where(a => Convert.ToInt32(a.Attribute("Id").Value) == countryId).Select(b => new Country()
            {
                CountryId = Convert.ToInt32(b.Attribute("Id").Value),
                CountryName = b.Attribute("Name").Value
            }).FirstOrDefault();

            if(country == null)
            {
                return StatusCode(StatusCodes.Status200OK, new ReturnResponse()
                {
                    StatusCode = Utils.StatusCode_NotFound,
                    StatusMessage = Utils.StatusMessage_NotFound
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ReturnResponse()
            {
                StatusCode = Utils.StatusCode_Success,
                StatusMessage = Utils.StatusMessage_Success,
                ObjectValue = country
            });
        }

        [HttpPost]
        public async Task<ActionResult<ReturnResponse>> PostCountry([FromBody] CountryRequest countryRequest)
        {
            if(countryRequest == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ReturnResponse()
                {
                    StatusCode = Utils.StatusCode_ObjectNull,
                    StatusMessage = Utils.StatusMessage_ObjectNull
                });
            }

            if(string.IsNullOrWhiteSpace(countryRequest.CountryName))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ReturnResponse()
                {
                    StatusCode = Utils.StatusCode_NameEmpty,
                    StatusMessage = Utils.StatusMessage_NameEmpty
                });
            }

            var countries = XDocument.Load(_hostingEnvironment.ContentRootPath + @"\CountryDb\Countries.xml");
            var lastCountryId = countries.Element("Countries").Elements("Country").Count();           
            lastCountryId++;
            var country = new Country()
            {
                CountryId = lastCountryId,
                CountryName = countryRequest.CountryName
            };

            countries.Element("Countries").Add(new XElement("Country", new XAttribute("Id", country.CountryId), new XAttribute("Name", country.CountryName)));
            countries.Save(_hostingEnvironment.ContentRootPath + @"\CountryDb\Countries.xml");

            return StatusCode(StatusCodes.Status200OK, new ReturnResponse()
            {
                StatusCode = Utils.StatusCode_Success,
                StatusMessage = Utils.StatusMessage_Success,
                ObjectValue = country
            });
        }
    }
}
