using Microsoft.JSInterop;

namespace TTB.Support
{

    public delegate Task HandleCallEvent();
    public class CallHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<CallHandler>? _objRef;
        public event HandleCallEvent Notify; 

        public CallHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            _objRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync(
                "initializeCallHandler",
                _objRef
            );
        }

        [JSInvokable]
        public async Task HandleCallEvent()
        {
            await Notify.Invoke();
        }

        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}
