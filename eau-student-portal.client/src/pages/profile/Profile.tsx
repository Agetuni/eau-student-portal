import { User, Mail, GraduationCap, Calendar } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Button } from '@/components/ui/button';
import { useAuth } from '@/hooks/useAuth';
import { Separator } from '@/components/ui/separator';

export function Profile() {
  const { user } = useAuth();

  const getUserInitials = () => {
    if (user?.name) {
      return user.name
        .split(' ')
        .map((n) => n[0])
        .join('')
        .toUpperCase()
        .slice(0, 2);
    }
    if (user?.email) {
      return user.email[0].toUpperCase();
    }
    return 'U';
  };

  const getUserDisplayName = () => {
    return user?.name || user?.email?.split('@')[0] || 'User';
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Profile</h1>
        <p className="text-muted-foreground mt-2">
          Manage your profile information and settings
        </p>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Personal Information</CardTitle>
            <CardDescription>Your account details</CardDescription>
          </CardHeader>
          <CardContent className="space-y-6">
            <div className="flex items-center gap-4">
              <Avatar className="h-20 w-20">
                <AvatarImage src={user?.avatar} alt={getUserDisplayName()} />
                <AvatarFallback className="text-lg">{getUserInitials()}</AvatarFallback>
              </Avatar>
              <div>
                <h3 className="text-lg font-semibold">{getUserDisplayName()}</h3>
                <p className="text-sm text-muted-foreground">Student</p>
              </div>
            </div>

            <Separator />

            <div className="space-y-4">
              <div className="flex items-center gap-3">
                <Mail className="h-4 w-4 text-muted-foreground" />
                <div>
                  <p className="text-sm font-medium">Email</p>
                  <p className="text-sm text-muted-foreground">{user?.email}</p>
                </div>
              </div>

              {user?.studentId && (
                <div className="flex items-center gap-3">
                  <GraduationCap className="h-4 w-4 text-muted-foreground" />
                  <div>
                    <p className="text-sm font-medium">Student ID</p>
                    <p className="text-sm text-muted-foreground">{user.studentId}</p>
                  </div>
                </div>
              )}

              <div className="flex items-center gap-3">
                <Calendar className="h-4 w-4 text-muted-foreground" />
                <div>
                  <p className="text-sm font-medium">Member Since</p>
                  <p className="text-sm text-muted-foreground">
                    {new Date().toLocaleDateString()}
                  </p>
                </div>
              </div>
            </div>

            <Button className="w-full">Edit Profile</Button>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Academic Information</CardTitle>
            <CardDescription>Your academic details</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <p className="text-sm font-medium">Program</p>
              <p className="text-sm text-muted-foreground">Aviation Management</p>
            </div>
            <Separator />
            <div className="space-y-2">
              <p className="text-sm font-medium">Year</p>
              <p className="text-sm text-muted-foreground">Sophomore</p>
            </div>
            <Separator />
            <div className="space-y-2">
              <p className="text-sm font-medium">GPA</p>
              <p className="text-sm text-muted-foreground">3.7</p>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}

