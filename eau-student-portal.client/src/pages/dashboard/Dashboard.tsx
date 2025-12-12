import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';

export function Dashboard() {

  return (
    <div className="space-y-6">
      {/* Welcome Section */}
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Welcome Back!</h1>
        <p className="text-muted-foreground mt-2">
          Here's what's happening with your studies today.
        </p>
      </div>

    </div>
  );
}

