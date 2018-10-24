import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Client } from '../../models/client';

@Component({
  selector: 'app-create-client',
  templateUrl: './create-client.component.html',
  styleUrls: ['./create-client.component.css']
})
export class CreateClientComponent implements OnInit {

  clientForm: FormGroup;
  client: Client;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {

    this.client = new Client("", "");

    this.clientForm = new FormGroup({
      clientName: new FormControl("", {
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
      this.client.clientName = f.clientName;
      this.client.email = f.email;
    });
  }

  onSubmit(fClient) {
  }
}
