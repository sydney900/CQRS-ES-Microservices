import { IResource } from "./iResource";

export class Client implements IResource {
  public id: number;
  public clientName: string;
  public email: string;
}
