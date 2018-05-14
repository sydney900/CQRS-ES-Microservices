import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements OnInit {

  clients: Observable<any>;

  constructor(private router: Router) {
  }

  ngOnInit() {
  }
  
  onAddClient() {
    //navigate to creat client
    console.log("navigate to creat client");

    this.router.navigate(["/create-client"]);
  }

  onDeleteClient(id) {
    //delete the client
    console.log("delete the client");

    this.apollo.mutate({
      mutation: gqlDeleteClient,
      refetchQueries: [{ query: gqlGetAllClients }],
      variables: {
        id: id
      }
    }).subscribe(({ data }) => {
        console.log('Deleted client: ', data);
    }, (error) => {
      console.log('there was an error deleting client', error);
    });
  }
  

}
