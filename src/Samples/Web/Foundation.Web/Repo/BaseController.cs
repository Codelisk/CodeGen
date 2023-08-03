using Attributes.WebAttributes.Controller;
using Foundation.Web.Manager;
using Foundation.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web.Repo
{
    [Authorize]
    [Route("/[controller]")]
    public abstract class BaseController<TManager, TController> : Microsoft.AspNetCore.Mvc.Controller
        where TManager : IManager
        where TController : Microsoft.AspNetCore.Mvc.Controller
    {
        protected TManager Manager { get; }
        private ILogger<TController> Logger { get; }

        protected BaseController(
            [NotNull] TManager mgr,
            [NotNull] ILogger<TController> logger)
        {
            Manager = mgr;
            Logger = logger;
        }

        protected async Task<ActionResult<TResult>> DoWithLoggingAsync<TResult>(Func<Task<TResult>> asyncFunc)
        {
            var methodName = asyncFunc.Method.Name;
            Logger.LogTrace($"Executing {methodName} ...");

            var result = await asyncFunc();

            Logger.LogDebug($"Executed {methodName}");

            return Ok(result);
        }

        protected async Task<ActionResult> DoWithLoggingAsync(Func<Task> asyncFunc)
        {
            var methodName = asyncFunc.Method.Name;
            Logger.LogTrace($"Executing {methodName} ...");

            await asyncFunc();

            Logger.LogDebug($"Executed {methodName}");

            return Ok();
        }

        protected int GetSellerId()
        {
            return int.Parse(User.FindFirst("SellerId").Value);

        }
        protected int GetUserId()
        {
            if (User.Identity.Name != null)
            {
                var userId = Int32.Parse(User.Identity.Name);
                return userId;
            }
            else
            {
                throw new Exception("User Id null");
            }

        }

        protected int? GetPrinterId()
        {
            int printerId = 0;
            if (User.FindFirst("PrinterId") != null)
            {
                int.TryParse(User.FindFirst("PrinterId").Value, out printerId);
                if (printerId == 0)
                {
                    return null;
                }
                return printerId;
            }
            else
            {
                throw new Exception("Printer Id null");
            }
        }
        protected int GetCashboxId()
        {
            if (User.FindFirst("CashboxId") != null)
            {
                return int.Parse(User.FindFirst("CashboxId").Value);
            }
            else
            {
                throw new Exception("CashboxId is null");
            }

        }

        protected int GetSignatureType()
        {
            if (User.FindFirst("SignatureType") != null)
            {
                return int.Parse(User.FindFirst("SignatureType").Value);
            }
            else
            {
                throw new Exception("SignatureType is null");
            }

        }

        protected int GetSellerTypeId()
        {
            if (User.FindFirst("SellerTypId") != null)
            {
                return int.Parse(User.FindFirst("SellerTypId").Value);
            }
            else
            {
                throw new Exception("SellerTypId is null");
            }

        }

        protected string GetCountryCode()
        {
            if (User.FindFirst("CountryCode") != null)
            {
                return User.FindFirst("CountryCode").Value;
            }
            else
            {
                throw new Exception("CountryCode is null");
            }
        }
        protected string GetCultureCode()
        {
            if (User.FindFirst("CultureCode") != null)
            {
                return User.FindFirst("CultureCode").Value;
            }
            else
            {
                throw new Exception("CultureCode is null");
            }
        }

        protected string GetLanguage()
        {
            if (User.FindFirst("CultureCode") != null)
            {
                var cultureCode = User.FindFirst("CultureCode").Value;
                if (cultureCode != null)
                {
                    var split = cultureCode.Split('-');
                    return split[0];
                }
            }

            throw new Exception("CultureCode is null");
        }

        protected string GetUserName()
        {
            if (User.FindFirst("UserName") != null)
            {
                return User.FindFirst("UserName").Value;
            }
            else
            {
                throw new Exception("CountryCode is null");
            }
        }

        protected void SetSeller(SellerBase obj)
        {
            obj.SellerId = GetSellerId();
        }
    }
}
