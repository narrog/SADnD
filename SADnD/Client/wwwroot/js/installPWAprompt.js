window.addEventListener('beforeinstallprompt', function (e) {
    e.preventDefault();
    window.PWADeferredPrompt = e;

    // Nach ca 10min durchgehender Benutzung soll die Frage für die Installation angezeigt werden.
    setTimeout(() => {
        BlazorPWA.ComponentReference.invokeMethodAsync("ActivateInstallButton")
            .catch(err => console.error("Error invoking Blazor method:", err));
    }, 600000);
});

window.BlazorPWA = {
    ComponentReference: null,
    StoreComponent: function (componentReference) {
        BlazorPWA.ComponentReference = componentReference;
    },
    InstallPWA: function () {
        if (window.PWADeferredPrompt) {
            window.PWADeferredPrompt.prompt();
            window.PWADeferredPrompt.userChoice
                .then(function (choiceResult) {
                    BlazorPWA.ComponentReference.invokeMethodAsync("ChoiceResult", choiceResult.outcome);
                    window.PWADeferredPrompt = null;
                });
        }
    }
};