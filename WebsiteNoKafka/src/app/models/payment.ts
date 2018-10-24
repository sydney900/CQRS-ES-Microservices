import { IResource } from './iResource';

export class Payment implements IResource {
  public id: number;
  constructor(
    public bSB?: string,
    public accountNumber?: string,
    public accountName?: string,
    public reference?: string,
    public paymentAmount?: number
  ) { }
}
