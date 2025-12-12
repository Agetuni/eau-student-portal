import { useLocation, useNavigate } from 'react-router-dom';
import {
  LayoutDashboard,
  User,
} from 'lucide-react';
import { cn } from '@/utils';
import { Button } from '@/components/ui/button';
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet';

interface MobileSidebarProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

interface NavItem {
  title: string;
  href: string;
  icon: React.ComponentType<{ className?: string }>;
  badge?: number;
}

const navItems: NavItem[] = [
  {
    title: 'Dashboard',
    href: '/dashboard',
    icon: LayoutDashboard,
  },
  {
    title: 'Profile',
    href: '/profile',
    icon: User,
  },
];

export function MobileSidebar({ open, onOpenChange }: MobileSidebarProps) {
  const location = useLocation();
  const navigate = useNavigate();

  const isActive = (href: string) => {
    if (href === '/dashboard') {
      return location.pathname === '/dashboard' || location.pathname === '/';
    }
    return location.pathname.startsWith(href);
  };

  const handleNavigate = (href: string) => {
    navigate(href);
    onOpenChange(false);
  };

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent side="left" className="w-64 p-0">
        <SheetHeader className="border-b px-4 py-4">
          <SheetTitle>Navigation</SheetTitle>
        </SheetHeader>
        <nav className="p-4 space-y-1">
          {navItems.map((item) => {
            const Icon = item.icon;
            const active = isActive(item.href);
            return (
              <Button
                key={item.href}
                variant={active ? 'secondary' : 'ghost'}
                className={cn(
                  'w-full justify-start gap-3',
                  active && 'bg-primary/10 text-primary font-medium'
                )}
                onClick={() => handleNavigate(item.href)}
              >
                <Icon className={cn('h-5 w-5 shrink-0', active && 'text-primary')} />
                <span className="flex-1 text-left">{item.title}</span>
                {item.badge && (
                  <span className="ml-auto rounded-full bg-primary px-2 py-0.5 text-xs font-medium text-primary-foreground">
                    {item.badge}
                  </span>
                )}
              </Button>
            );
          })}
        </nav>
      </SheetContent>
    </Sheet>
  );
}

