using Microsoft.JSInterop;

namespace TTB.Client.Support
{

    public delegate Task HandlePasteEvent();
    public class PasteHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<PasteHandler>? _objRef;
        public event HandlePasteEvent Notify; 

        public PasteHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            _objRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync(
                "initializePasteHandler",
                _objRef
            );
        }

        [JSInvokable]
        public async Task HandlePasteEvent()
        {
            await Notify.Invoke();
        }

        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}
