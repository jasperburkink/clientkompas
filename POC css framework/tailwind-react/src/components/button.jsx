import '../index.css';

export function Button(props) {
  var type = props.type
  if(type === "NotSollid"){
    return (<button className={"btnNotSollid " +  (props.className)} href={props.href} onClick={props.onClick}>{props.text}</button>)
  }else{
    return (<button className={"btnSollid " +  (props.className)} href={props.href} onClick={props.onClick}>{props.text}</button>);
  }
}