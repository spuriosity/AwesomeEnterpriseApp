﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AwesomeEnterpriseApp.Models;
using AwesomeEnterpriseApp.BusinessLogic;
using AwesomeEnterpriseApp.Models.UI;
//using AwesomeEnterpriseApp.DataAccessLayer;
using System.Web.Mvc;

namespace AwesomeEnterpriseApp.Controllers
{
    public class RestaurantFinder : Controller
    {
       

        

        public String GetMessage(String radius)
        {
            return "Info" + radius;

           
        }


    }
}