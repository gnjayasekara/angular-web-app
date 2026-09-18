import {
  ChangeDetectorRef,
  Component,
  inject,
} from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

import { LoginRequest } from '../../../models/auth.models';
import { AuthService } from '../../../core/services/auth.service';



@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

 

  isLoading = false;
  errorMessage = '';

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  constructor() {
    this.loginForm.valueChanges.subscribe(() => {
      if (this.errorMessage) {
        this.errorMessage = '';
      }
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const request: LoginRequest = {
      companyCode: this.loginForm.value.email!,
      username: this.loginForm.value.email!,
      password: this.loginForm.value.password!,
    };

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(request).subscribe({
      next: (response) => {
        this.isLoading = false;

        console.log('Login successful:', response);

        this.router.navigate(['/dashboard']);
      },

      error: (error) => {
        this.isLoading = false;

        console.error('Login failed:', error);

        if (error.status === 401) {
          this.errorMessage = 'Invalid email or password.';
        } else {
          this.errorMessage =
            error.error?.message ||
            'Login failed. Please try again.';
        }

        this.cdr.detectChanges();
      },
    });
  }
}