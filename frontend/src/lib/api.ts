const API_URL = 'http://localhost:5167';

export function getToken(): string | null {
    return localStorage.getItem('token');
}

export function setToken(token: string): void {
    localStorage.setItem('token', token);
}

export function removeToken(): void {
    localStorage.removeItem('token');
}


export async function api(path: string, options: RequestInit = {}) {
    const token = getToken();
    const headers: Record<string, string> = {
        ...options.headers as Record<string, string>,
    };

    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    
    if (options.body && !(options.body instanceof FormData)) {
        headers['Content-Type'] = 'application/json';
    }

    const res = await fetch(`${API_URL}${path}`, {
        ...options,
        headers
    });

    return res;
}