using System;
using System.Linq;
using System.Threading.Tasks;
using Game2048Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.ViewModels.Common;

namespace Game2048Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }

        public IActionResult ActionWrap(Func<ResultBase> func)
        {
            ResultBase result = new ResultBase();
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = string.Empty;
                    ModelState.Values.AsEnumerable().ToList().ForEach(it =>
                    {
                        if (it.Errors.Count > 0)
                        {
                            error += it.Errors[0].ErrorMessage;
                        }
                    });
                    throw new ArgumentException(error);
                }

                result = func();
                result.Status = 0;
                result.Msg = string.Empty;
            }
            catch (Exception ex)
            {
                result.Status = 1;
                _logger.LogError(ex, ex.Message);
                result.Msg = ex.Message;
            }

            return Ok(result);
        }


        public async Task<IActionResult> ActionWrapAsync(Func<Task<ResultBase>> func)
        {
            ResultBase result = new ResultBase();
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = string.Empty;
                    ModelState.Values.AsEnumerable().ToList().ForEach(it =>
                    {
                        if (it.Errors.Count > 0)
                        {
                            error += it.Errors[0].ErrorMessage;
                        }
                    });
                    throw new ArgumentException(error);
                }

                result = await func();
                result.Status = 0;
                result.Msg = string.Empty;
            }
            catch (Exception ex)
            {
                result.Status = 1;
                _logger.LogError(ex, ex.Message);
                result.Msg = ex.Message;
            }

            return Ok(result);
        }
    }
}