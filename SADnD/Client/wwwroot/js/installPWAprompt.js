window.addEventListener('beforeinstallprompt', function (e) {
    e.preventDefault();

    if (localStorage.getItem('installPromptDenied') === 'true') {
        return;
    }

    window.PWADeferredPrompt = e;

    let intervalStep = 100;
    let intervalTimeout = 5000;
    let intervalCounter = 0;

    let intervalId = setInterval(() => {
        if (BlazorPWA.ComponentReference === null) {
            intervalCounter += intervalStep;
            if (intervalCounter > intervalTimeout) {
                clearInterval(intervalId);
            }
        }
        else {
            BlazorPWA.ComponentReference.invokeMethodAsync("ActivateInstallButton")
                .catch(err => console.error("Error invoking Blazor method:", err));
            clearInterval(intervalId);
        }
    }, intervalStep);
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
                    if (choiceResult.outcome === 'dismissed') {
                        localStorage.setItem('installPromptDenied', 'true');
                    }
                    BlazorPWA.ComponentReference.invokeMethodAsync("DeactivateInstallButton");
                    window.PWADeferredPrompt = null;
                });
        }
    }
};