import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authService.photoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }
  // next - our request is successful and we receive data from api
  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {

   // const token = localStorage.getItem('token');
    // !! - true or false
    // if token variable is not empty, !! returns true, if token variable is empty - false
   // return !!token;
    return this.authService.loggedIn();

  }

  logout() {
    // when the user logs out, his info must be removed from the localStorage along with token and set it's value to null
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('Logged out');
    this.router.navigate(['home']);

  }

}
