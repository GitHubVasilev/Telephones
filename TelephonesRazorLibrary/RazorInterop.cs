using Microsoft.JSInterop;

namespace TelephonesRazorLibrary
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class RazorInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        public RazorInterop(IJSRuntime jsRuntime)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/TelephonesRazorLibrary/razorLibrary.js").AsTask());
        }

        public async ValueTask AlertMessage(string msg) 
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("alertMessage", msg);
        }

        public async ValueTask ReloadPage()
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("reloadPage");
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}