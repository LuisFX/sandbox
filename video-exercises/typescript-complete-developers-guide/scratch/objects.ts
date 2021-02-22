const profile = {
    name: 'lfx',
    age: 20,
    coords : {
        lat: 0,
        lng: 15
    },
    setAge(age: number) : void {
        this.age = age
    }
}

const { age } : { age: number } = profile
const { name } : { name: string } = profile
const {
    coords: { lat, lng }
}: { coords : { lat: number; lng: number } } = profile

console.log(name)
export default profile