﻿using Microsoft.AspNetCore.Mvc;

namespace ProjectPortfolio.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}