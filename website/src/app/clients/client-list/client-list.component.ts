import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements OnInit {

  clients: Observable<any>;

  constructor(private router: Router) { }

  ngOnInit() {
    this.clients = new Observable();
  }

  onAddClient() {
    //navigate to creat client
    console.log("navigate to creat client");

    this.router.navigate(["/create-client"]);
  }

  onDeleteClient(id) {
    //delete the client
    console.log("delete the client");
  }
}
