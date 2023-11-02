using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var html = new HtmlContentBuilder();
        var type = helper.ViewData.ModelExplorer.ModelType;
        html.AppendHtml("<form>");
        foreach (var property in type.GetProperties())
        {
            html.AppendHtml(Division(property, helper.ViewData.Model!));
        }
        html.AppendHtml("<form/>");
        return html;
    }

    public static IHtmlContent Division(PropertyInfo property, object model)
    {
        var html = new TagBuilder("div");
        html.InnerHtml.AppendHtml(GetLabel(property));

        if (!property.PropertyType.IsEnum)
            html.InnerHtml.AppendHtml(GetInput(property));
        else
            html.InnerHtml.AppendHtml(GetSelect(property));

        html.InnerHtml.AppendHtml(Validate(property, model));
        return html;
    }

    private static IHtmlContent Validate(PropertyInfo property, object? model)
    {
        var html = new TagBuilder("span");
        html.InnerHtml.AppendHtml(string.Empty);
        if (model == null)
        {
            return html;
        }

        var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
        
        foreach (ValidationAttribute validationAttribute in validationAttributes)
        {
            if (!validationAttribute.IsValid(property.GetValue(model)))
            {
                html.InnerHtml.AppendHtml(validationAttribute.ErrorMessage!);
                return html;
            }
        }
        return html;
    }

    private static IHtmlContent GetLabel(PropertyInfo property)
    {
        var html = new TagBuilder("label");
        var display = property.GetCustomAttribute<DisplayAttribute>();

        if (display?.Name is null) 
            html.InnerHtml.AppendHtml(SeparateName(property.Name));
        else
            html.InnerHtml.AppendHtml(display.Name);

        html.Attributes.Add("for", property.Name);
        return html;
    }

    private static string SeparateName(string name)
    {
        return Regex.Replace(name, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
    }

    private static IHtmlContent GetInput(PropertyInfo property)
    {
        var html = new TagBuilder("input");
        html.Attributes.Add("type", property.PropertyType == typeof(int) ? "number" : "text");
        html.Attributes.Add("id", property.Name);
        html.Attributes.Add("name", property.Name);
        return html;
    }

    private static IHtmlContent GetSelect(PropertyInfo property)
    {
        var html = new TagBuilder("select");
        var values = property.PropertyType.GetEnumValues();
        html.Attributes.Add("id", property.Name);
        foreach (var value in values)
        {
            html.InnerHtml.AppendHtml($"<option value=\"{value}\">{value}<option/>");
        }
        return html;
    }
} 