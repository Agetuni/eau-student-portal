import { Bot } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { cn } from '@/utils';

interface ChatButtonProps {
  onClick: () => void;
  isOpen?: boolean;
}

export function ChatButton({ onClick, isOpen }: ChatButtonProps) {
  if (isOpen) return null;

  return (
    <Button
      onClick={onClick}
      className={cn(
        'fixed bottom-6 right-6 z-40 h-14 w-14 rounded-full shadow-lg',
        'bg-primary text-primary-foreground hover:bg-primary/90',
        'flex items-center justify-center gap-2',
        'transition-all duration-200 hover:scale-105'
      )}
      aria-label="Open AI Chat"
    >
      <Bot className="h-6 w-6" />
      <span className="sr-only">AI Chat</span>
    </Button>
  );
}

