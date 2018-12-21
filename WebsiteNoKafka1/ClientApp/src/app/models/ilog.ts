export interface ILog<T> {
    log(message: string): T;
    info(message: string): T;
    warn(message: string): T;
    error(message: string): T;
    debug(message: string): T;
}
