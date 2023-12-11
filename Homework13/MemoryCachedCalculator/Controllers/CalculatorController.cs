using System.Diagnostics.CodeAnalysis;
using MemoryCachedCalculator.Dto;
using MemoryCachedCalculator.Services;
using MemoryCachedCalculator.Services.MathCalculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCachedCalculator.Controllers;

public class CalculatorController : Controller
{
    private readonly IMathCalculatorService _mathCalculatorService;
    private readonly IMemoryCache _memoryCache;

    public CalculatorController(IMathCalculatorService mathCalculatorService, IMemoryCache memoryCache)
    {
        _mathCalculatorService = mathCalculatorService;
        _memoryCache = memoryCache;
    }
        
    [HttpGet]
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult<CalculationMathExpressionResultDto>> CalculateMathExpression(string expression)
    {
        var result = await _mathCalculatorService.CalculateMathExpressionAsync(expression);
        return Json(result);
    }
}