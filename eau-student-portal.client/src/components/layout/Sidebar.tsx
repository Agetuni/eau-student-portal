import { useLocation, useNavigate } from 'react-router-dom';
import {
  LayoutDashboard,
  User,
  ChevronLeft,
  ChevronRight,
} from 'lucide-react';
import { cn } from '@/utils';
import { Button } from '@/components/ui/button';
import { ScrollArea } from '@/components/ui/scroll-area';

interface SidebarProps {
  isCollapsed: boolean;
  onToggle: () => void;
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

export function Sidebar({ isCollapsed, onToggle }: SidebarProps) {
  const location = useLocation();
  const navigate = useNavigate();

  const isActive = (href: string) => {
    if (href === '/dashboard') {
      return location.pathname === '/dashboard' || location.pathname === '/';
    }
    return location.pathname.startsWith(href);
  };

  return (
    <aside
      className={cn(
        'hidden lg:flex flex-col border-r bg-background transition-all duration-300',
        isCollapsed ? 'w-16' : 'w-64'
      )}
    >
      {/* Collapse Toggle */}
      <div className="flex h-16 items-center justify-between border-b px-4">
        {!isCollapsed && (
          <h2 className="text-lg font-semibold text-primary">Navigation</h2>
        )}
        <Button
          variant="ghost"
          size="icon"
          onClick={onToggle}
          className="ml-auto"
          aria-label={isCollapsed ? 'Expand sidebar' : 'Collapse sidebar'}
        >
          {isCollapsed ? (
            <ChevronRight className="h-4 w-4" />
          ) : (
            <ChevronLeft className="h-4 w-4" />
          )}
        </Button>
      </div>

      {/* Navigation Items */}
      <ScrollArea className="flex-1">
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
                  isCollapsed && 'justify-center px-0',
                  active && 'bg-primary/10 text-primary font-medium'
                )}
                onClick={() => navigate(item.href)}
              >
                <Icon className={cn('h-5 w-5 shrink-0', active && 'text-primary')} />
                {!isCollapsed && (
                  <>
                    <span className="flex-1 text-left">{item.title}</span>
                    {item.badge && (
                      <span className="ml-auto rounded-full bg-primary px-2 py-0.5 text-xs font-medium text-primary-foreground">
                        {item.badge}
                      </span>
                    )}
                  </>
                )}
              </Button>
            );
          })}
        </nav>
      </ScrollArea>

    </aside>
  );
}

