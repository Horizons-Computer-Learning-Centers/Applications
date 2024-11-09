import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../shared/api/auth-api';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'horizons-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
})
export class ConfirmEmailComponent implements OnInit {
  errorMessage = '';
  successMessage = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('userId');
    console.log(userId);
    const token = this.route.snapshot.paramMap.get('token');
    console.log(token);

    // if (userId && token) {
    //   this.authService.confirmEmail(userId, token).subscribe((response) => {});
    // }

    if (userId && token) {
      this.authService
        .confirmEmail(userId, token)
        .pipe(
          catchError((error) => {
            // Handle the error here
            console.error('Error confirming email:', error);
            // Optionally show an error message to the user
            this.errorMessage =
              'There was an error confirming your email. Please try again later.';
            // Return an empty observable to complete the observable stream
            return of(null);
          })
        )
        .subscribe((response) => {
          if (response) {
            // Handle successful confirmation here
            console.log('Email confirmed successfully:', response);
            this.successMessage = 'Your email has been successfully confirmed!';
          }
        });
    }
  }
}
