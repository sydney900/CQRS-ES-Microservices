import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ClientService } from '../../services/client.service';
import { Client } from 'src/app/models/client';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements OnInit {

  clients: Observable<Client>;

  constructor(private router: Router, private clientService: ClientService) { }

  ngOnInit() {
    this.clients = this.clientService.getAll();
  }

  onAddClient() {
    // navigate to creat client
    console.log('navigate to creat client');

    this.router.navigate(['/create-client']);
  }

  onDeleteClient(id) {
    // delete the client
    console.log('delete the client');

    this.clientService.delete(id).subscribe();
  }
}
