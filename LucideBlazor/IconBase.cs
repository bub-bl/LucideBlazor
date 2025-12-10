using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace LucideBlazor;

public abstract class IconBase : IComponent
{
    [Parameter] public string? ClassName { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? Props { get; set; }
    
    private RenderHandle Handle;

    public void Attach(RenderHandle renderHandle)
    {
        Handle = renderHandle;
    }

    protected void RenderIcon(RenderTreeBuilder builder, string svgContent)
    {
        var counter = 0;

        builder.OpenElement(0, "svg");

        builder.AddAttribute(counter++, "class", ClassName);

        if (Props != null)
        {
            foreach (var attribute in Defaults.DefaultAttributes)
            {
                if (!Props.ContainsKey(attribute.Key))
                    builder.AddAttribute(counter++, attribute.Key, attribute.Value);
            }

            builder.AddMultipleAttributes(counter++, Props);
        }
        else
            builder.AddMultipleAttributes(counter++, Defaults.DefaultAttributes);

        builder.AddMarkupContent(counter++, svgContent);

        builder.CloseElement();
    }

    protected abstract void HandleRender(RenderTreeBuilder builder);

    public Task SetParametersAsync(ParameterView parameters)
    {
        foreach (var parameter in parameters)
        {
            switch (parameter.Name)
            {
                case nameof(ClassName):
                    ClassName = (string)parameter.Value;
                    break;
                
                default:
                    Props ??= new Dictionary<string, object?>();
                    Props[parameter.Name] = parameter.Value;
                    
                    break;
            }
        }
        
        Handle.Render(HandleRender);

        return Task.CompletedTask;
    }
}