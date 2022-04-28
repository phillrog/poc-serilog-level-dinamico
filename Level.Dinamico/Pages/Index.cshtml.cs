using Level.Dinamico.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog.Events;

namespace Level.Dinamico.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Serilog.ILogger _logger;
        private readonly ISerilogLoggingService _logLevelSwitch;
        public string LevelAtual { get; set; }

        public IndexModel(ISerilogLoggingService logLevelSwitch, Serilog.ILogger logger)
        {
            _logLevelSwitch = logLevelSwitch;
            LevelAtual = _logLevelSwitch.GetLoggingLevelSwitch().MinimumLevel.ToString();
            _logger = logger;
        }

        public void OnGetLimparConsole()
        {
            Console.Clear();
        }

        public async Task<IActionResult> OnPost(string level)
        {
            _logLevelSwitch.SetLoggingLevel((LogEventLevel)Enum.Parse(typeof(LogEventLevel), level, true));

            LevelAtual = _logLevelSwitch.GetLoggingLevelSwitch().MinimumLevel.ToString();
            return Page();
        }

        public void OnGetNovoLog()
        {
            _logger.Write((LogEventLevel)Enum.Parse(typeof(LogEventLevel), LevelAtual, true), $"Este é um novo log do tipo: {LevelAtual}");
        }
    }
}