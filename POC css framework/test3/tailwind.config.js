/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./dist/*.{html,js}"],
  theme: {
    extend: {
      colors: {
      'mainBlue' : '#148CB8',
      'mainGray' : '#E3E3E3',
      'mainLightGray' : '#F9F9F9',

      'subBlue1' : '#117DA3',
      'subBlue2' : '#0c6a8c',
      'subGray1' : '#CECECE',
      'subGray2' : '#B3B3B3'
      },
      width: {
        '50px' : '50px',
        '150px' : '150px',
        '200px' : '200px',
        '324px' : '324px', 
        '748px' : '748px',
        'sidebarBlue' : '374px',
        'sidebarGray' : '474px',
        '50rem' : '50rem'
      },
      minWidth: {
        'sidebarBlue' : '374px',
        'sidebarGray' : '474px'
      },
      height: {
        '50px' : '50px',
        '65px' : '65px',
        '100px' : '100px',
        '150px' : '150px',
        '324px' : '324px', 
        '400px' : '400px',
        '500px' : '500px',
        '698px' : '698px',
        '748px' : '748px',
        'sidebar' : '125vh'
      },
      maxHeight: {
        '300px' : '300px'
      },
      margin: {
        '15px' : '15px',
        '50px' : '50px',
        '100px' : '100px',
        '150px' : '150px',
        '200px' : '200px',
        '250px' : '250px',
        'sidebarGray' : '430px',
        'line' : '185px',
        'lineEmptySidebar' : '785px',
        'darkLine' : '90px'
      },
      padding: {
        '50px' : '50px'
      },
      borderRadius: {
        "sidebarT" : '50px 100%',
        "sidebarB" : '200px 100%',
        "50%" : "50%"
      },
      rotate: {
        "line" : "50deg",
        "darkLine" : "30deg"
      },
      textUnderlineOffset: {
        3: '3px'
      }
    },
  },
  plugins: [],
}

