import React, { ImgHTMLAttributes, LabelHTMLAttributes } from 'react';
import '../../styles/common/picture.css';

interface PictureProps extends React.ImgHTMLAttributes<HTMLImageElement> {
    source: string
}

export const Picture = (props: PictureProps) => (
    <img className={props.className} src={props.source} />
);