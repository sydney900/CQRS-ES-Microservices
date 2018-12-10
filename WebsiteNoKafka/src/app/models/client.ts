import { IResource } from "./iResource";

export class Client implements IResource {
  public id: string;
  public name: string;
  public email: string;
  public version: number = 1;
}
