import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forbidden',
  templateUrl: './forbidden.component.html',
  styleUrls: ['./forbidden.component.css']
})
export class ForbiddenComponent implements OnInit {

  public message: string;

  constructor() {
    this.message = "403: You don't have permission to access this.";
  }

  ngOnInit() {
  }

}
