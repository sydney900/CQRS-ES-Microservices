import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Client } from '../../models/client';
import { ClientService } from 'src/app/services/client.service';

@Component({
  selector: 'app-create-client',
  templateUrl: './create-client.component.html',
  styleUrls: ['./create-client.component.css']
})
export class CreateClientComponent implements OnInit {

  clientForm: FormGroup;
  client: Client;

  constructor(private fb: FormBuilder, private clientService: ClientService) { }

  ngOnInit() {

    this.client = new Client();

    this.clientForm = new FormGroup({
      name: new FormControl("", {
        validators: Validators.required,
        updateOn: 'submit'
      }),
      clientPassword: new FormControl("", {
        validators: [Validators.required, Validators.min(5)],
        updateOn: 'submit'
      }),
      email: new FormControl("", {
        validators: Validators.email,
        updateOn: 'submit'
      })
    });

    this.clientForm.valueChanges.subscribe(f => {
      this.client.name = f.name;
      this.client.email = f.email;
    });
  }

  onSubmit(fClient) {
    this.clientService.create(fClient);
  }
}
