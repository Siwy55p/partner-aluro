using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Services;

namespace partner_aluro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string>GetSessionInfo()
        {
            List<string> sessionInfo = new List<string>();
            if(string.IsNullOrWhiteSpace(HttpContext.Session.GetString(Services.SessionVariables.SessionKeyUsername)))
            {
                HttpContext.Session.SetString(SessionKeyEnum.SessionKeyUsername.ToString(), "Current User");
                HttpContext.Session.SetString(SessionKeyEnum.SessionKeySessionId.ToString(), Guid.NewGuid().ToString());
            }
            var username = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
            var sessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

            sessionInfo.Add(username);
            sessionInfo.Add(sessionId);

            return sessionInfo;
        }
    }
}
