export interface LoginCredentials {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

/**
 * Mock login service that simulates an API call
 */
export async function login(credentials: LoginCredentials): Promise<LoginResponse> {
  // Simulate API delay
  await new Promise((resolve) => setTimeout(resolve, 500));

  // Mock validation - accept any email/password combination
  // In a real app, this would make an actual API call
  if (credentials.email && credentials.password) {
    return {
      token: `mock-token-${Date.now()}-${Math.random().toString(36).substring(7)}`,
    };
  }

  throw new Error('Invalid credentials');
}

