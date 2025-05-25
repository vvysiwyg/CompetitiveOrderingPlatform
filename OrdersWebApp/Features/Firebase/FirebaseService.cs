using Microsoft.JSInterop;

namespace OrdersWebApp.Features.Firebase
{
    public class FirebaseService
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<FirebaseService> _objectReference;
        public event Action<string> OnFCMTokenReceived;
        public event Func<int, Task> OnNewOrderReceived;

        public FirebaseService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public FirebaseService() { }

        public async Task GetFCMTokenAsync()
        {
            _objectReference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("getFCMToken", _objectReference);
        }

        public async Task InitializeFirebaseMessaging()
        {
            await _jsRuntime.InvokeVoidAsync("initializeFirebaseMessaging");
        }

        public async Task InitializeMessageListener()
        {
            await _jsRuntime.InvokeVoidAsync("initializeMessageListener");
        }

        [JSInvokable]
        public void ReceiveFCMToken(string token)
        {
            Console.WriteLine("ReceiveFCMToken. " + token);
            Console.WriteLine($"OnFCMTokenReceived is null ? " + (OnFCMTokenReceived == null));
            OnFCMTokenReceived?.Invoke(token);
        }

        [JSInvokable]
        public async Task ReceiveNewOrder(int orderId)
        {
            Console.WriteLine($"Receive new order. {orderId}");
            Console.WriteLine($"OnNewOrderReceived is null ? " + (OnNewOrderReceived == null));

            if (OnNewOrderReceived != null)
                await OnNewOrderReceived(orderId);
        }
    }
}
