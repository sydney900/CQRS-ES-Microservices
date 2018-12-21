import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent implements OnInit {

  public message: string;

  constructor() {
    this.message = '401: You have no rights to access this. Please Login';
  }

  ngOnInit() {
  }

}
