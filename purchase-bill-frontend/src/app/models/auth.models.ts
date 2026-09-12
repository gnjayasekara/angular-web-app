export interface LoginRequest {
  companyCode: string;
  username: string;
  password: string;
}

export interface LoginLocation {
  locationCode: string;
  locationName: string;
  stockHandle: number;
  address: string;
  phone: string;
  status: number;
}

export interface LoginResponse {
  token: string;
  userCode: string;
  displayName: string;
  email: string;
  companyCode: string;
  locations: LoginLocation[];
}