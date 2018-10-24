import { IResource } from "./iResource";

export class Client implements IResource {
  constructor(
    public clientName: string,
    public email: string
  ) { }

  public id: number;
}
