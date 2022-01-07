export class FileService{
    static async ConvertToBase64(file: File): Promise<string | ArrayBuffer>{
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            console.log(reader);
            reader.onload = () => resolve(reader.result);
            reader.onerror = (error) => reject(error)
        })
    }
}