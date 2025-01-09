import NavItem from "types/common/NavItem";

export default interface GetMenuByUserDto {
    userid: string; 
    menuitems: NavItem[];
}