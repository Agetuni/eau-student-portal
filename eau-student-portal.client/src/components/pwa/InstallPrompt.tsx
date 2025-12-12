import { useEffect, useState } from 'react';
import { Download } from 'lucide-react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';

interface BeforeInstallPromptEvent extends Event {
  prompt: () => Promise<void>;
  userChoice: Promise<{ outcome: 'accepted' | 'dismissed' }>;
}

const STORAGE_KEY = 'pwa-install-dismissed';

export function InstallPrompt() {
  const [deferredPrompt, setDeferredPrompt] = useState<BeforeInstallPromptEvent | null>(null);
  const [showDialog, setShowDialog] = useState(false);
  const [isInstalled, setIsInstalled] = useState(() => {
    // Initialize state by checking if app is already installed
    if (typeof window !== 'undefined') {
      return window.matchMedia('(display-mode: standalone)').matches;
    }
    return false;
  });

  useEffect(() => {
    // Early return if already installed
    if (isInstalled) {
      return;
    }

    // Check if user has dismissed the prompt
    const dismissed = localStorage.getItem(STORAGE_KEY);
    if (dismissed === 'true') {
      return;
    }

    // Listen for the beforeinstallprompt event
    const handleBeforeInstallPrompt = (e: Event) => {
      e.preventDefault();
      setDeferredPrompt(e as BeforeInstallPromptEvent);
      setShowDialog(true);
    };

    window.addEventListener('beforeinstallprompt', handleBeforeInstallPrompt);

    // Check if app was installed
    const handleAppInstalled = () => {
      setIsInstalled(true);
      setShowDialog(false);
      setDeferredPrompt(null);
    };

    window.addEventListener('appinstalled', handleAppInstalled);

    return () => {
      window.removeEventListener('beforeinstallprompt', handleBeforeInstallPrompt);
      window.removeEventListener('appinstalled', handleAppInstalled);
    };
  }, [isInstalled]);

  const handleInstall = async () => {
    if (!deferredPrompt) return;

    // Show the install prompt
    await deferredPrompt.prompt();

    // Wait for the user to respond
    const { outcome } = await deferredPrompt.userChoice;

    if (outcome === 'accepted') {
      setIsInstalled(true);
    }

    setDeferredPrompt(null);
    setShowDialog(false);
  };

  const handleDontAskAgain = () => {
    localStorage.setItem(STORAGE_KEY, 'true');
    setShowDialog(false);
    setDeferredPrompt(null);
  };

  // Don't show if already installed or no prompt available
  if (isInstalled || !deferredPrompt || !showDialog) {
    return null;
  }

  return (
    <Dialog open={showDialog} onOpenChange={setShowDialog}>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <div className="flex items-center gap-3 mb-2">
            <img
              src="/logo.png"
              alt="App Icon"
              className="h-12 w-12 rounded-lg"
              onError={(e) => {
                const target = e.target as HTMLImageElement;
                target.style.display = 'none';
              }}
            />
            <DialogTitle>Install Aviation University Portal</DialogTitle>
          </div>
          <DialogDescription>
            Install our app for a better experience! Get quick access, offline support, and a
            native app-like experience right from your home screen.
          </DialogDescription>
        </DialogHeader>
        <DialogFooter className="flex-col sm:flex-row gap-2">
          <Button
            variant="outline"
            onClick={handleDontAskAgain}
            className="w-full sm:w-auto"
          >
            Don't ask again
          </Button>
          <Button onClick={handleInstall} className="w-full sm:w-auto">
            <Download className="mr-2 h-4 w-4" />
            Install App
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

